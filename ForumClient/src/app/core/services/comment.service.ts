import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import {
  Comment,
  CreateCommentPayload,
  UpdateCommentPayload
} from '../models/comment.model';
import { VoteType } from '../types/vote-type';

@Injectable({ providedIn: 'root' })
export class CommentService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/comments`;

  getComment(id: number): Observable<Comment> {
    return this.http.get<Comment>(`${this.baseUrl}/${id}`);
  }

  getCommentsForPost(postId: number, page = 1, pageSize = 10): Observable<Comment[]> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<Comment[]>(`${environment.apiUrl}/posts/${postId}/comments`, { params });
  }

  createComment(payload: CreateCommentPayload): Observable<Comment> {
    return this.http.post<Comment>(this.baseUrl, payload);
  }

  updateComment(id: number, payload: UpdateCommentPayload): Observable<Comment> {
    return this.http.put<Comment>(`${this.baseUrl}/${id}`, payload);
  }

  deleteComment(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  vote(id: number, voteType: VoteType): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/${id}/vote`, voteType);
  }
}
