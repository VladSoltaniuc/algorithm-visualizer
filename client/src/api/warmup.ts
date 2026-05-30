const fire = (url: string, body?: unknown) =>
  fetch(url, body !== undefined
    ? { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(body) }
    : { method: 'GET' }
  ).catch(() => {});

export function warmupServer() {
  fire('/api/sort/bubble-sort',                    [1, 2]);
  fire('/api/find/kadane',                          [1, 2]);
  fire('/api/dynamicprog/fibonacci/2');
  fire('/api/misc/bit-manipulation/1');
  fire('/api/pattern/kmp',                         { text: 'ab', pattern: 'a' });
  fire('/api/tree/inorder',                         [1]);
  fire('/api/backtracking/permutations',            [1, 2]);
  fire('/api/graph/bfs',                            { nodeCount: 2, edges: [[0, 1]], startNode: 0 });
}
