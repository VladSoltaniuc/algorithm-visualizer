import type { AlgorithmStep } from "../../types";
import { useStepPlayer } from "../../hooks/useStepPlayer";
import "./VisControls.css";

interface Props {
  steps: AlgorithmStep[];
  onRun: () => void;
  disabled?: boolean;
  children: (step: AlgorithmStep) => React.ReactNode;
}

export default function VisControls({
  steps,
  onRun,
  disabled,
  children,
}: Props) {
  const { step, currentStep, speed, setSpeed, isPlaying, total } =
    useStepPlayer(steps);

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
          {children(step)}
          <p className="step-description">{step.description}</p>
          <span className="step-counter">
            Step {currentStep + 1} / {total}
          </span>
        </>
      )}
    </div>
  );
}
