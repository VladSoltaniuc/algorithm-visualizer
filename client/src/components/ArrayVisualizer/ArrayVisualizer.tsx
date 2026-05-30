import { useEffect, useRef, useState } from "react";
import type { AlgorithmStep } from "../../types";
import "./ArrayVisualizer.css";
import "../VisControls/VisControls.css";

interface Props {
  steps: AlgorithmStep[];
  onRun: () => void;
  disabled?: boolean;
  inputControls?: React.ReactNode;
  speed: number;
  isPaused?: boolean;
  onComplete?: () => void;
}

export default function ArrayVisualizer({
  steps,
  onRun: _onRun,
  disabled: _disabled,
  inputControls,
  speed,
  isPaused = false,
  onComplete,
}: Props) {
  const [currentStep, setCurrentStep] = useState(0);
  const [isPlaying, setIsPlaying] = useState(false);
  const timerRef = useRef<ReturnType<typeof setTimeout> | null>(null);
  const onCompleteRef = useRef(onComplete);
  onCompleteRef.current = onComplete;

  const step = steps[currentStep];

  useEffect(() => {
    setCurrentStep(0);
    setIsPlaying(steps.length > 1);
  }, [steps]);

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

    timerRef.current = setTimeout(() => {
      setCurrentStep((s) => s + 1);
    }, speed);

    return () => {
      if (timerRef.current) clearTimeout(timerRef.current);
    };
  }, [isPlaying, currentStep, steps.length, speed, isPaused]);

  const maxVal = step ? Math.max(...step.array.map(Math.abs), 1) : 1;

  return (
    <div className="visualizer">
      {inputControls && (
        <div className="vis-controls">
          {inputControls}
        </div>
      )}

      {step && (
        <>
          <div className="squares-container">
            {step.array.map((value, idx) => {
              const isHighlighted = step.highlightIndices?.includes(idx);
              const isSorted = step.sortedIndices?.includes(idx);
              const height = Math.max(40, (Math.abs(value) / maxVal) * 220);

              let className = "square";
              if (isSorted) className += " sorted";
              else if (isHighlighted) className += " highlighted";

              return (
                <div
                  key={idx}
                  className={className}
                  style={{
                    height: `${height}px`,
                    transition: `all ${Math.min(speed * 0.8, 400)}ms ease`,
                  }}
                >
                  <span className="square-value">{value}</span>
                </div>
              );
            })}
          </div>
          <div
            className={`step-info${currentStep === steps.length - 1 ? " final" : ""}`}
          >
            {step.description}
          </div>
          <span className="step-counter">
            Step {currentStep + 1} / {steps.length}
          </span>
        </>
      )}
    </div>
  );
}
