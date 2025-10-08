import { AsyncPipe, DatePipe, NgFor, NgIf } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { catchError, map, of, shareReplay, switchMap, tap } from 'rxjs';

import { Comment } from '../../core/models/comment.model';
import { Post } from '../../core/models/post.model';
import { CommentService } from '../../core/services/comment.service';
import { PostService } from '../../core/services/post.service';
import { CommentThreadComponent } from '../../shared/components/comment-thread/comment-thread.component';

@Component({
  selector: 'app-post-detail',
  standalone: true,
  imports: [NgIf, NgFor, AsyncPipe, DatePipe, RouterLink, CommentThreadComponent],
  templateUrl: './post-detail.component.html',
  styleUrl: './post-detail.component.css'
})
export class PostDetailComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly postService = inject(PostService);
  private readonly commentService = inject(CommentService);

  private readonly postState = signal<{ loading: boolean; error: string | null; data: Post | null }>({
    loading: true,
    error: null,
    data: null
  });

  private readonly commentsState = signal<{ loading: boolean; error: string | null; data: Comment[] }>({
    loading: true,
    error: null,
    data: []
  });

  readonly post$ = this.route.paramMap.pipe(
    map(params => params.get('slug')),
    switchMap(slug => {
      if (!slug) {
        this.postState.set({ loading: false, error: 'Post not found.', data: null });
        return of(null);
      }

      this.postState.set({ loading: true, error: null, data: null });

      return this.postService.getPostBySlug(slug).pipe(
        tap(post => this.postState.set({ loading: false, error: null, data: post })),
        catchError(() => {
          this.postState.set({ loading: false, error: 'Post not found.', data: null });
          return of(null);
        })
      );
    }),
    shareReplay({ bufferSize: 1, refCount: true })
  );

  readonly comments$ = this.post$.pipe(
    switchMap(post => {
      if (!post) {
        this.commentsState.set({ loading: false, error: null, data: [] });
        return of([]);
      }

      this.commentsState.set({ loading: true, error: null, data: [] });

      return this.commentService.getCommentsForPost(post.id, 1, 50).pipe(
        tap(comments => this.commentsState.set({ loading: false, error: null, data: comments })),
        catchError(() => {
          this.commentsState.set({ loading: false, error: 'Unable to load comments right now.', data: [] });
          return of([]);
        })
      );
    })
  );

  readonly postError = computed(() => this.postState().error);
  readonly commentsError = computed(() => this.commentsState().error);
  readonly isLoadingPost = computed(() => this.postState().loading);
  readonly isLoadingComments = computed(() => this.commentsState().loading);

  trackByTag = (_: number, tag: string) => tag;

  toCategoryLink(categoryName: string): string {
    return categoryName.trim().toLowerCase().replace(/\s+/g, '-');
  }
}
