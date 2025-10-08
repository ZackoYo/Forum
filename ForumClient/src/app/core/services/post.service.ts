import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import {
  CreatePostPayload,
  GetPostsParams,
  Post,
  PostSummary,
  UpdatePostPayload
} from '../models/post.model';
import { VoteType } from '../types/vote-type';

@Injectable({ providedIn: 'root' })
export class PostService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/posts`;
  private readonly defaultParams: Required<GetPostsParams> = {
    page: 1,
    pageSize: 10,
    sortBy: 'created',
    descending: true
  };

  getPosts(params: GetPostsParams = {}): Observable<PostSummary[]> {
    return this.http.get<PostSummary[]>(this.baseUrl, {
      params: this.createParams(params)
    });
  }

  getPostById(id: number): Observable<Post> {
    return this.http.get<Post>(`${this.baseUrl}/${id}`);
  }

  getPostBySlug(slug: string): Observable<Post> {
    return this.http.get<Post>(`${this.baseUrl}/slug/${slug}`);
  }

  getPostsByCategory(categoryId: number, params: GetPostsParams = {}): Observable<PostSummary[]> {
    return this.http.get<PostSummary[]>(`${this.baseUrl}/category/${categoryId}`, {
      params: this.createParams(params)
    });
  }

  createPost(payload: CreatePostPayload): Observable<Post> {
    return this.http.post<Post>(this.baseUrl, payload);
  }

  updatePost(id: number, payload: UpdatePostPayload): Observable<Post> {
    return this.http.put<Post>(`${this.baseUrl}/${id}`, payload);
  }

  deletePost(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  vote(id: number, voteType: VoteType): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/${id}/vote`, voteType);
  }

  private createParams(params: GetPostsParams): HttpParams {
    const merged = { ...this.defaultParams, ...params };
    let httpParams = new HttpParams();

    Object.entries(merged).forEach(([key, value]) => {
      if (value !== undefined && value !== null) {
        httpParams = httpParams.set(key, String(value));
      }
    });

    return httpParams;
  }
}
