import type { AlgorithmStep } from '../types';

const BASE = '/api/tree';

async function post(url: string, body: number[]): Promise<AlgorithmStep[]> {
  const res = await fetch(url, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(body),
  });
  if (!res.ok) throw new Error((await res.text()) || `Server error: ${res.status}`);
  return res.json();
}

async function postJson(url: string, body: unknown): Promise<AlgorithmStep[]> {
  const res = await fetch(url, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(body),
  });
  if (!res.ok) throw new Error((await res.text()) || `Server error: ${res.status}`);
  return res.json();
}

export const treeApi: Record<string, (...args: unknown[]) => Promise<AlgorithmStep[]>> = {
  inorder: (arr: unknown) => post(`${BASE}/inorder`, arr as number[]),
  preorder: (arr: unknown) => post(`${BASE}/preorder`, arr as number[]),
  postorder: (arr: unknown) => post(`${BASE}/postorder`, arr as number[]),
  levelOrder: (arr: unknown) => post(`${BASE}/level-order`, arr as number[]),
  bstInsertSearch: (arr: unknown, target?: unknown) =>
    post(`${BASE}/bst-insert-search/${target}`, arr as number[]),
  height: (arr: unknown) => post(`${BASE}/height`, arr as number[]),
  lca: (arr: unknown, nodeA?: unknown, nodeB?: unknown) =>
    post(`${BASE}/lca/${nodeA}/${nodeB}`, arr as number[]),
  invert: (arr: unknown) => post(`${BASE}/invert`, arr as number[]),
  validateBst: (arr: unknown) => post(`${BASE}/validate-bst`, arr as number[]),
  diameter: (arr: unknown) => post(`${BASE}/diameter`, arr as number[]),
  huffman: (text: unknown) => postJson(`${BASE}/huffman`, { text, pattern: '' }),
};
