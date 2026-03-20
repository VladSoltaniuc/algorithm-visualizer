import { createContext, useContext, useState, useCallback } from "react";

const LS_KEY = "learned_algorithms";

function getStored(): Set<string> {
  try {
    const raw = localStorage.getItem(LS_KEY);
    return raw ? new Set(JSON.parse(raw) as string[]) : new Set();
  } catch {
    return new Set();
  }
}

interface LearnedContextValue {
  isLearned: (key: string) => boolean;
  toggle: (key: string) => void;
}

const LearnedContext = createContext<LearnedContextValue>({
  isLearned: () => false,
  toggle: () => {},
});

export function LearnedProvider({ children }: { children: React.ReactNode }) {
  const [learned, setLearned] = useState<Set<string>>(getStored);

  const toggle = useCallback((key: string) => {
    setLearned((prev) => {
      const next = new Set(prev);
      if (next.has(key)) next.delete(key);
      else next.add(key);
      try {
        localStorage.setItem(LS_KEY, JSON.stringify([...next]));
      } catch {}
      return next;
    });
  }, []);

  const isLearned = useCallback((key: string) => learned.has(key), [learned]);

  return (
    <LearnedContext.Provider value={{ isLearned, toggle }}>
      {children}
    </LearnedContext.Provider>
  );
}

export const useLearned = () => useContext(LearnedContext);
