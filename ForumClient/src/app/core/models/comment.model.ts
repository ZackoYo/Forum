export interface Comment {
  id: number;
  content: string;
  authorId: string;
  authorName: string;
  postId: number;
  parentCommentId: number | null;
  votesCount: number;
  replies: Comment[];
  createdOn: string;
  modifiedOn?: string | null;
  isDeleted: boolean;
  deletedOn?: string | null;
}

export interface CreateCommentPayload {
  content: string;
  postId: number;
  parentCommentId?: number | null;
}

export interface UpdateCommentPayload {
  content: string;
}