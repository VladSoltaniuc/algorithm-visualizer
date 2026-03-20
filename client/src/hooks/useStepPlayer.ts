import { useEffect, useRef, useState } from "react";
import type { AlgorithmStep } from "../types";

export function useStepPlayer(steps: AlgorithmStep[], autoPlay = true) {
  const [currentStep, setCurrentStep] = useState(0);
  const [isPlaying, setIsPlaying] = useState(false);
  const [speed, setSpeed] = useState(1000);
  const timerRef = useRef<ReturnType<typeof setTimeout> | null>(null);

  const step = steps[currentStep] ?? null;

  useEffect(() => {
    setCurrentStep(0);
    setIsPlaying(autoPlay && steps.length > 1);
  }, [steps, autoPlay]);

  useEffect(() => {
    if (!isPlaying) {
      if (timerRef.current) clearTimeout(timerRef.current);
      return;
    }
    if (currentStep >= steps.length - 1) {
      setIsPlaying(false);
      return;
    }
    timerRef.current = setTimeout(() => setCurrentStep((s) => s + 1), speed);
    return () => {
      if (timerRef.current) clearTimeout(timerRef.current);
    };
  }, [isPlaying, currentStep, steps.length, speed]);

  return { step, currentStep, setCurrentStep, isPlaying, setIsPlaying, speed, setSpeed, total: steps.length };
}
