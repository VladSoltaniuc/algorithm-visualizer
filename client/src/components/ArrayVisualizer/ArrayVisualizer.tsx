import { useEffect, useRef, useState } from "react";
import type { AlgorithmStep } from "../../types";
import "./ArrayVisualizer.css";

interface Props {
  steps: AlgorithmStep[];
  onRun: () => void;
  disabled?: boolean;
}

export default function ArrayVisualizer({ steps, onRun, disabled }: Props) {
  const [currentStep, setCurrentStep] = useState(0);
  const [isPlaying, setIsPlaying] = useState(false);
  const [speed, setSpeed] = useState(500);
  const timerRef = useRef<ReturnType<typeof setTimeout> | null>(null);

  const step = steps[currentStep];

  useEffect(() => {
    setCurrentStep(0);
    setIsPlaying(steps.length > 1);
  }, [steps]);

  useEffect(() => {
    if (!isPlaying) {
      if (timerRef.current) clearTimeout(timerRef.current);
      return;
    }

    if (currentStep >= steps.length - 1) {
      setIsPlaying(false);
      return;
    }

    timerRef.current = setTimeout(() => {
      setCurrentStep((s) => s + 1);
    }, speed);

    return () => {
      if (timerRef.current) clearTimeout(timerRef.current);
    };
  }, [isPlaying, currentStep, steps.length, speed]);

  const maxVal = step ? Math.max(...step.array.map(Math.abs), 1) : 1;

  return (
    <div className="visualizer">
      <div className="vis-controls">
        <label className="speed-control">
          Speed
          <input
            type="range"
            min={50}
            max={1500}
            step={50}
            value={1550 - speed}
            onChange={(e) => setSpeed(1550 - Number(e.target.value))}
          />
        </label>
        <button
          className="run-btn"
          onClick={onRun}
          disabled={disabled || isPlaying}
        >
          ▶︎
        </button>
      </div>

      {step && (
        <>
          <div className="squares-container">
            {step.array.map((value, idx) => {
              const isHighlighted = step.highlightIndices?.includes(idx);
              const isSorted = step.sortedIndices?.includes(idx);
              const height = Math.max(40, (Math.abs(value) / maxVal) * 300);

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
          <p className="step-description">
            {step.description}
            {step && (
              <span className="step-counter">
                Step {currentStep + 1} / {steps.length}
              </span>
            )}
          </p>
        </>
      )}
    </div>
  );
}
