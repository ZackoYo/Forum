import { Routes } from '@angular/router';

import { HomeComponent } from './features/home/home.component';
import { CategoriesComponent } from './features/categories/categories.component';
import { CategoryPageComponent } from './features/categories/category-page.component';
import { PostDetailComponent } from './features/posts/post-detail.component';
import { NewPostComponent } from './features/posts/new-post.component';
import { NotFoundComponent } from './shared/components/not-found/not-found.component';

export const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'categories',
    component: CategoriesComponent
  },
  {
    path: 'categories/:slug',
    component: CategoryPageComponent
  },
  {
    path: 'posts/new',
    component: NewPostComponent
  },
  {
    path: 'posts/:slug',
    component: PostDetailComponent
  },
  {
    path: '**',
    component: NotFoundComponent
  }
];
