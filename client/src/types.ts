export interface AlgorithmStep {
  stepNumber: number;
  array: number[];
  description: string;
  highlightIndices: number[];
  sortedIndices: number[];
  patternArray?: number[];
  patternOffset?: number;
  patternHighlightIndex?: number;
  isNumericArray?: boolean;
  textHash?: number;
  patternHash?: number;
  pArray?: number[];
  manacherCenter?: number;
  manacherRight?: number;

  // DP matrix visualization (e.g. LCS)
  dpMatrix?: number[][];
  rowHeaders?: string;
  colHeaders?: string;
  highlightRow?: number;
  highlightCol?: number;
  backtrackPath?: number[];
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
