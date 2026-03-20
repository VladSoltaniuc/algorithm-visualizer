export interface AlgorithmStep {
  stepNumber: number;
  array: number[];
  description: string;
  highlightIndices: number[];
  sortedIndices: number[];
}

export interface AlgorithmConfig {
  name: string;
  slug: string;
  endpoint: string;
  category: string;
  defaultInput: string;
  description: string;
  inputLabel?: string;
  inputType?: 'numbers' | 'text' | 'number' | 'graph';
  needsTarget?: boolean;
  targetLabel?: string;
  defaultTarget?: number;
  needsWindowSize?: boolean;
  defaultWindowSize?: number;
  needsPattern?: boolean;
  patternLabel?: string;
  defaultPattern?: string;
}
