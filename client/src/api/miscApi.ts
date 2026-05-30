import type { AlgorithmStep } from '../types';

const BASE = '/api/misc';

async function postJson(url: string, body: unknown): Promise<AlgorithmStep[]> {
  const res = await fetch(url, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(body) });
  if (!res.ok) throw new Error((await res.text()) || `Server error: ${res.status}`);
  return res.json();
}

async function get(url: string): Promise<AlgorithmStep[]> {
  const res = await fetch(url);
  if (!res.ok) throw new Error((await res.text()) || `Server error: ${res.status}`);
  return res.json();
}

export const miscApi: Record<string, (...args: unknown[]) => Promise<AlgorithmStep[]>> = {
  reversal: (text: unknown) => postJson(`${BASE}/reversal`, { text, pattern: '' }),
  huffman: (text: unknown) => postJson(`${BASE}/huffman`, { text, pattern: '' }),
  bitManipulation: (n: unknown) => get(`${BASE}/bit-manipulation/${n}`),
};
