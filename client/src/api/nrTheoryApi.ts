import type { AlgorithmStep } from '../types';

const BASE = '/api/nrtheory';

async function get(url: string): Promise<AlgorithmStep[]> {
  const res = await fetch(url);
  if (!res.ok) throw new Error((await res.text()) || `Server error: ${res.status}`);
  return res.json();
}

async function post(url: string, body: unknown): Promise<AlgorithmStep[]> {
  const res = await fetch(url, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(body),
  });
  if (!res.ok) throw new Error((await res.text()) || `Server error: ${res.status}`);
  return res.json();
}

export const nrTheoryApi: Record<string, (...args: unknown[]) => Promise<AlgorithmStep[]>> = {
  sieve: (n: unknown) => get(`${BASE}/sieve/${n}`),
  gcd: (input: unknown) => {
    const parts = String(input).split(',').map(Number);
    return get(`${BASE}/gcd/${parts[0]}/${parts[1]}`);
  },
  extendedGcd: (input: unknown) => {
    const parts = String(input).split(',').map(Number);
    return get(`${BASE}/extended-gcd/${parts[0]}/${parts[1]}`);
  },
  fastExponentiation: (input: unknown) => {
    const parts = String(input).split(',').map(Number);
    return get(`${BASE}/fast-exp/${parts[0]}/${parts[1]}/${parts[2]}`);
  },
  modularArithmetic: (input: unknown) => {
    const parts = String(input).split(',').map(Number);
    return get(`${BASE}/modular/${parts[0]}/${parts[1]}/${parts[2]}`);
  },
  primeFactorization: (n: unknown) => get(`${BASE}/prime-factorization/${n}`),
  fibonacciMatrix: (n: unknown) => get(`${BASE}/fibonacci-matrix/${n}`),
  chineseRemainder: (remainders: unknown, _?: unknown, moduliStr?: unknown) => {
    const moduli = typeof moduliStr === 'string' ? moduliStr.split(',').map(Number) : [];
    return post(`${BASE}/chinese-remainder`, [remainders, moduli]);
  },
  bitManipulation: (n: unknown) => get(`${BASE}/bit-manipulation/${n}`),
  catalan: (n: unknown) => get(`${BASE}/catalan/${n}`),
};
