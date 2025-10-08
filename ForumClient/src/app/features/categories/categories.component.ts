import { AsyncPipe, NgFor, NgIf, isPlatformServer } from '@angular/common';
import { Component, PLATFORM_ID, computed, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Observable, catchError, map, of, tap } from 'rxjs';

import { Category } from '../../core/models/category.model';
import { CategoryService } from '../../core/services/category.service';

@Component({
  selector: 'app-categories',
  standalone: true,
  imports: [NgIf, NgFor, AsyncPipe, RouterLink],
  templateUrl: './categories.component.html',
  styleUrl: './categories.component.css'
})
export class CategoriesComponent {
  private readonly categoryService = inject(CategoryService);
  private readonly platformId = inject(PLATFORM_ID);
  private readonly isServer = isPlatformServer(this.platformId);

  private readonly state = signal<{ loading: boolean; error: string | null; data: Category[] }>({
    loading: true,
    error: null,
    data: []
  });

  private readonly categoriesSource$: Observable<Category[]> = this.isServer
    ? of([])
    : this.categoryService.getCategories();

  readonly categories$ = this.categoriesSource$.pipe(
    map(categories => [...categories].sort((a, b) => a.displayOrder - b.displayOrder)),
    tap(categories => this.state.set({ loading: false, error: null, data: categories })),
    catchError(() => {
      this.state.set({ loading: false, error: 'Unable to load categories right now.', data: [] });
      return of([]);
    })
  );

  readonly hasError = computed(() => this.state().error);
  readonly isLoading = computed(() => this.state().loading);

  trackByCategory = (_: number, category: Category) => category.id;
  trackBySubCategory = (_: number, category: Category) => category.id;
}
