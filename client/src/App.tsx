import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import Navigation from "./components/Navigation/Navigation";
import AlgorithmPage from "./pages/AlgorithmPage/AlgorithmPage";
import NotFoundPage from "./pages/NotFoundPage/NotFoundPage";
import { LearnedProvider } from "./context/LearnedContext";
import "./App.css";

function App() {
  return (
    <LearnedProvider>
      <BrowserRouter>
        <Navigation />
        <Routes>
          <Route
            path="/"
            element={<Navigate to="/array/bubble-sort" replace />}
          />
          <Route path="/:category/:slug" element={<AlgorithmPage />} />
          <Route path="/404" element={<NotFoundPage />} />
          <Route path="*" element={<Navigate to="/404" replace />} />
        </Routes>
      </BrowserRouter>
    </LearnedProvider>
  );
}

export default App;
