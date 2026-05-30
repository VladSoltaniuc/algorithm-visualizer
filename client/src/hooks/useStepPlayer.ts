import { useEffect, useRef, useState } from "react";
import type { AlgorithmStep } from "../types";

export function useStepPlayer(
  steps: AlgorithmStep[],
  speed: number,
  isPaused = false,
  onComplete?: () => void,
  autoPlay = true,
) {
  const [currentStep, setCurrentStep] = useState(0);
  const [isPlaying, setIsPlaying] = useState(false);
  const timerRef = useRef<ReturnType<typeof setTimeout> | null>(null);
  const onCompleteRef = useRef(onComplete);
  onCompleteRef.current = onComplete;

  const step = steps[currentStep] ?? null;

  useEffect(() => {
    setCurrentStep(0);
    setIsPlaying(autoPlay && steps.length > 1);
  }, [steps, autoPlay]);

  useEffect(() => {
    if (!isPlaying || isPaused) {
      if (timerRef.current) clearTimeout(timerRef.current);
      return;
    }
    if (currentStep >= steps.length - 1) {
      setIsPlaying(false);
      onCompleteRef.current?.();
      return;
    }
    timerRef.current = setTimeout(() => setCurrentStep((s) => s + 1), speed);
    return () => {
      if (timerRef.current) clearTimeout(timerRef.current);
    };
  }, [isPlaying, currentStep, steps.length, speed, isPaused]);

  return { step, currentStep, setCurrentStep, isPlaying, setIsPlaying, total: steps.length };
}
