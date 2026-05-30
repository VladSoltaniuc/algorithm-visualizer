import type { AlgorithmStep } from "../../types";
import { useStepPlayer } from "../../hooks/useStepPlayer";
import "./VisControls.css";

interface Props {
  steps: AlgorithmStep[];
  disabled?: boolean;
  hideDescription?: boolean;
  inputControls?: React.ReactNode;
  speed: number;
  isPaused?: boolean;
  onComplete?: () => void;
  children: (step: AlgorithmStep) => React.ReactNode;
}

export default function VisControls({
  steps,
  disabled: _disabled,
  hideDescription,
  inputControls,
  speed,
  isPaused = false,
  onComplete,
  children,
}: Readonly<Props>) {
  const { step, currentStep, isPlaying: _isPlaying, total } = useStepPlayer(steps, speed, isPaused, onComplete);

  return (
    <div className="visualizer">
      {inputControls && (
        <div className="vis-controls">
          {inputControls}
        </div>
      )}

      {step && (
        <>
          {children(step)}
          {!hideDescription && (
            <p className="step-description">{step.description}</p>
          )}
          <span className="step-counter">
            Step {currentStep + 1} / {total}
          </span>
        </>
      )}
    </div>
  );
}
