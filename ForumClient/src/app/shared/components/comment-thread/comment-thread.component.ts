import { DatePipe, NgFor, NgIf } from '@angular/common';
import { Component, forwardRef, Input } from '@angular/core';

import { Comment } from '../../../core/models/comment.model';

@Component({
  selector: 'app-comment-thread',
  standalone: true,
  imports: [NgFor, NgIf, DatePipe, forwardRef(() => CommentThreadComponent)],
  templateUrl: './comment-thread.component.html',
  styleUrl: './comment-thread.component.css'
})
export class CommentThreadComponent {
  @Input({ required: true }) comments: Comment[] | null = [];
  @Input() depth = 0;

  trackByComment = (_: number, comment: Comment) => comment.id;
}
