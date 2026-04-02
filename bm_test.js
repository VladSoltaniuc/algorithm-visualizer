// Boyer-Moore test cases
// For each case we POST to the API and verify:
//  - correct match positions are found
//  - at least one step uses the expected rule (bad-char / good-suffix / both)

const BASE = 'http://localhost:5286/api/string/boyer-moore';

const tests = [
  // ── bad-character-rule-dominant cases ────────────────────────────────────
  {
    name: 'BC: mismatch char not in pattern → full skip',
    text: 'ABCDEFGAB',
    pattern: 'XYZ',
    expectMatches: [],
    expectRule: 'bad-character',
  },
  {
    name: 'BC: single char pattern, no match',
    text: 'AABABCABC',
    pattern: 'X',
    expectMatches: [],
    expectRule: 'bad-character',
  },
  {
    name: 'BC: classic bad-char shift (mismatch char exists in pattern)',
    text: 'AABABCABC',
    pattern: 'ABC',
    expectMatches: [3, 6],
    expectRule: 'bad-character',
  },

  // ── good-suffix-rule-dominant cases ──────────────────────────────────────
  {
    // ANPANMAN in ANPANMANAP — good-suffix gives bigger shifts than bad-char
    name: 'GS: ANPANMAN pattern (Wikipedia example)',
    text: 'ANPANMANAP',
    pattern: 'ANPANMAN',
    expectMatches: [0],
    expectRule: 'good-suffix',
  },
  {
    // All-same chars: good-suffix is always dominant
    name: 'GS: repeated chars — good-suffix dominates',
    text: 'AAAAAAAAAA',
    pattern: 'AAA',
    expectMatches: [0,1,2,3,4,5,6,7],
    expectRule: 'good-suffix',
  },
  {
    name: 'GS: partial suffix match — good-suffix kicks in',
    text: 'ABCABABCAB',
    pattern: 'ABCAB',
    expectMatches: [0, 5],
    expectRule: 'good-suffix',
  },

  // ── both rules in play ───────────────────────────────────────────────────
  {
    name: 'BOTH: GCATCGCAGAGAGTATACAGTACG / GCAGAGAG (Knuth example)',
    text: 'GCATCGCAGAGAGTATACAGTACG',
    pattern: 'GCAGAGAG',
    expectMatches: [5],
    expectRule: null, // just verify correctness
  },
  {
    name: 'BOTH: multiple matches, mixed shifts',
    text: 'ABAAABABABAAAB',
    pattern: 'ABAB',
    expectMatches: [4, 6],
    expectRule: null,
  },
  {
    name: 'BOTH: no match, verify no false positives',
    text: 'XYZXYZXYZ',
    pattern: 'ABCD',
    expectMatches: [],
    expectRule: null,
  },
];

async function post(text, pattern) {
  const res = await fetch(BASE, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ text, pattern }),
  });
  if (!res.ok) throw new Error(`HTTP ${res.status}: ${await res.text()}`);
  return res.json();
}

function extractMatches(steps) {
  return steps
    .filter(s => s.description && s.description.startsWith('Pattern found at index '))
    .map(s => parseInt(s.description.replace('Pattern found at index ', ''), 10));
}

function usedRule(steps, rule) {
  return steps.some(s => s.description && s.description.includes(`(${rule} rule)`));
}

let passed = 0, failed = 0;

for (const t of tests) {
  try {
    const steps = await post(t.text, t.pattern);
    const matches = extractMatches(steps);
    const sortedMatches = [...matches].sort((a,b) => a-b);
    const sortedExpected = [...t.expectMatches].sort((a,b) => a-b);

    const matchOk = JSON.stringify(sortedMatches) === JSON.stringify(sortedExpected);
    const ruleOk = t.expectRule ? usedRule(steps, t.expectRule) : true;

    if (matchOk && ruleOk) {
      console.log(`✓  ${t.name}`);
      console.log(`   matches: [${sortedMatches}]${t.expectRule ? `  | rule "${t.expectRule}" used: yes` : ''}`);
      passed++;
    } else {
      console.log(`✗  ${t.name}`);
      if (!matchOk)
        console.log(`   FAIL matches: expected [${sortedExpected}] got [${sortedMatches}]`);
      if (!ruleOk)
        console.log(`   FAIL rule: expected "${t.expectRule}" to appear in steps, but it didn't`);
      // Print the mismatch/shift steps for debugging
      steps.filter(s => s.description && (s.description.includes('Mismatch') || s.description.includes('found')))
           .forEach(s => console.log(`   > ${s.description}`));
      failed++;
    }
  } catch (e) {
    console.log(`✗  ${t.name}  ERROR: ${e.message}`);
    failed++;
  }
}

console.log(`\n${passed + failed} tests  |  ${passed} passed  |  ${failed} failed`);
