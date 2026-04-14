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

  // Per-cell labels and secondary notes (e.g. Huffman char + binary code)
  labels?: string[];
  notes?: string[];

  // Full level-order with null gaps for dynamic tree shape reconstruction
  treeLevelOrder?: (number | null)[];

  // DP matrix visualization (e.g. LCS)
  dpMatrix?: number[][];
  rowHeaders?: string;
  colHeaders?: string;
  // Multi-character row/column labels (Knapsack, SubsetSum, LIS)
  rowLabels?: string[];
  colLabels?: string[];
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
  usecase: string;
  pros: string[];
  cons: string[];
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
  ytTutorial?: string;
  directed?: boolean;
}
