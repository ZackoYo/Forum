import { Routes } from '@angular/router';

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
