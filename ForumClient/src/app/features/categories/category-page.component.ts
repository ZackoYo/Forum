import { AsyncPipe, DatePipe, NgFor, NgIf } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { combineLatest, of } from 'rxjs';
import { catchError, map, shareReplay, switchMap, tap } from 'rxjs/operators';
import { takeUntilDestroyed, toObservable } from '@angular/core/rxjs-interop';

import { Category } from '../../core/models/category.model';
import { PostSummary } from '../../core/models/post.model';
import { CategoryService } from '../../core/services/category.service';
import { PostService } from '../../core/services/post.service';

@Component({
  selector: 'app-category-page',
  standalone: true,
  imports: [NgIf, NgFor, AsyncPipe, RouterLink, DatePipe],
  templateUrl: './category-page.component.html',
  styleUrl: './category-page.component.css'
})
export class CategoryPageComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly categoryService = inject(CategoryService);
  private readonly postService = inject(PostService);

  private readonly categoryState = signal<{ loading: boolean; error: string | null; data: Category | null }>({
    loading: true,
    error: null,
    data: null
  });

  private readonly postState = signal<{ loading: boolean; error: string | null; data: PostSummary[] }>({
    loading: true,
    error: null,
    data: []
  });

  private readonly pageSize = 10;
  readonly currentPage = signal(1);

  readonly category$ = this.route.paramMap.pipe(
    map(params => params.get('slug')),
    switchMap(slug => {
      if (!slug) {
        this.categoryState.set({ loading: false, error: 'Category not found.', data: null });
        return of(null);
      }

      this.categoryState.set({ loading: true, error: null, data: null });

      return this.categoryService.getCategoryBySlug(slug).pipe(
        tap(category => this.categoryState.set({ loading: false, error: null, data: category })),
        catchError(() => {
          this.categoryState.set({ loading: false, error: 'Category not found.', data: null });
          return of(null);
        })
      );
    }),
    shareReplay({ bufferSize: 1, refCount: true })
  );

  readonly posts$ = combineLatest([this.category$, toObservable(this.currentPage)]).pipe(
    switchMap(([category, page]) => {
      if (!category) {
        this.postState.set({ loading: false, error: null, data: [] });
        return of([]);
      }

      this.postState.set({ loading: true, error: null, data: [] });

      return this.postService.getPostsByCategory(category.id, { page, pageSize: this.pageSize }).pipe(
        tap(posts => this.postState.set({ loading: false, error: null, data: posts })),
        catchError(() => {
          this.postState.set({ loading: false, error: 'Unable to load posts at the moment.', data: [] });
          return of([]);
        })
      );
    })
  );

  readonly postsError = computed(() => this.postState().error);
  readonly canGoBack = computed(() => this.currentPage() > 1);
  readonly canGoForward = computed(() => this.postState().data.length === this.pageSize);

  constructor() {
    this.route.queryParamMap
      .pipe(
        takeUntilDestroyed(),
        map(params => Number(params.get('page')) || 1)
      )
      .subscribe(page => this.currentPage.set(Math.max(1, page)));
  }

  goToPreviousPage(): void {
    if (!this.canGoBack()) return;
    this.setPage(this.currentPage() - 1);
  }

  goToNextPage(): void {
    if (!this.canGoForward()) return;
    this.setPage(this.currentPage() + 1);
  }

  private setPage(page: number): void {
    if (page === this.currentPage()) return;

    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { page },
      queryParamsHandling: 'merge'
    });
  }

  trackByPost = (_: number, post: PostSummary) => post.id;
  trackByCategory = (_: number, category: Category) => category.id;
}
