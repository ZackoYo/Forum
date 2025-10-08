export type VoteType = 1 | -1;

export const VoteTypes = {
  Up: 1 as VoteType,
  Down: -1 as VoteType
} as const;
