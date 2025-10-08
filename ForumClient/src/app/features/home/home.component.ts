import { AsyncPipe, DatePipe, NgFor, NgIf, SlicePipe, isPlatformServer } from '@angular/common';
import { Component, PLATFORM_ID, computed, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Observable, catchError, map, of, tap } from 'rxjs';

import { Category } from '../../core/models/category.model';
import { PostSummary } from '../../core/models/post.model';
import { CategoryService } from '../../core/services/category.service';
import { PostService } from '../../core/services/post.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [NgIf, NgFor, AsyncPipe, DatePipe, RouterLink, SlicePipe],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  private readonly categoryService = inject(CategoryService);
  private readonly postService = inject(PostService);
  private readonly platformId = inject(PLATFORM_ID);
  private readonly isServer = isPlatformServer(this.platformId);

  private readonly categoryState = signal<{ loading: boolean; error: string | null; data: Category[] }>({
    loading: true,
    error: null,
    data: []
  });

  private readonly postState = signal<{ loading: boolean; error: string | null; data: PostSummary[] }>({
    loading: true,
    error: null,
    data: []
  });

  private readonly categoriesSource$: Observable<Category[]> = this.isServer
    ? of([])
    : this.categoryService.getCategories();

  private readonly postsSource$: Observable<PostSummary[]> = this.isServer
    ? of([])
    : this.postService.getPosts({ pageSize: 6 });

  readonly categories$ = this.categoriesSource$.pipe(
    map(categories => [...categories].sort((a, b) => a.displayOrder - b.displayOrder)),
    tap(categories => this.categoryState.set({ loading: false, error: null, data: categories })),
    catchError(() => {
      this.categoryState.set({ loading: false, error: 'Failed to load categories.', data: [] });
      return of([]);
    })
  );

  readonly latestPosts$ = this.postsSource$.pipe(
    tap(posts => this.postState.set({ loading: false, error: null, data: posts })),
    catchError(() => {
      this.postState.set({ loading: false, error: 'Failed to load posts.', data: [] });
      return of([]);
    })
  );

  readonly categoriesError = computed(() => this.categoryState().error);
  readonly postsError = computed(() => this.postState().error);

  readonly featuredCategories = computed(() => this.categoryState().data.slice(0, 6));

  trackByCategory = (_: number, category: Category) => category.id;
  trackByPost = (_: number, post: PostSummary) => post.id;
}
