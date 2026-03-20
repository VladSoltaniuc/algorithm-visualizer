import type { AlgorithmStep } from '../types';

const BASE = '/api/string';

async function postJson(url: string, body: unknown): Promise<AlgorithmStep[]> {
  const res = await fetch(url, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(body),
  });
  if (!res.ok) throw new Error((await res.text()) || `Server error: ${res.status}`);
  return res.json();
}

export const stringApi: Record<string, (...args: unknown[]) => Promise<AlgorithmStep[]>> = {
  linearSearch: (text: unknown, pattern?: unknown) =>
    postJson(`${BASE}/linear-search`, { text, pattern: pattern ?? '' }),
  kmp: (text: unknown, pattern?: unknown) =>
    postJson(`${BASE}/kmp`, { text, pattern: pattern ?? '' }),
  boyerMoore: (text: unknown, pattern?: unknown) =>
    postJson(`${BASE}/boyer-moore`, { text, pattern: pattern ?? '' }),
  rabinKarp: (text: unknown, pattern?: unknown) =>
    postJson(`${BASE}/rabin-karp`, { text, pattern: pattern ?? '' }),
  lcs: (text: unknown, pattern?: unknown) =>
    postJson(`${BASE}/lcs`, { text, pattern: pattern ?? '' }),
  longestPalindrome: (text: unknown) =>
    postJson(`${BASE}/longest-palindrome`, { text, pattern: '' }),
  anagramDetection: (text: unknown, pattern?: unknown) =>
    postJson(`${BASE}/anagram-detection`, { text, pattern: pattern ?? '' }),
  reversal: (text: unknown) =>
    postJson(`${BASE}/reversal`, { text, pattern: '' }),
  runLengthEncoding: (text: unknown) =>
    postJson(`${BASE}/run-length-encoding`, { text, pattern: '' }),
  levenshtein: (text: unknown, pattern?: unknown) =>
    postJson(`${BASE}/levenshtein`, { text, pattern: pattern ?? '' }),
};
