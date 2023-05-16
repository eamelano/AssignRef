const TestResults = lazy(() => import("../components/tests/TestResults"));
const TestPreview = lazy(() => import("../components/tests/TestPreview"));
const ResultViewer = lazy(() => import("../components/tests/ResultViewer"));

const tests = [
  {
    path: "/test/results",
    name: "Test Results",
    exact: true,
    element: TestResults,
    roles: ["Admin", "Assigner"],
    isAnonymous: false,
  },
  {
    path: "/test/:id/preview",
    name: "Test Preview",
    exact: true,
    element: TestPreview,
    roles: ["Admin", "Assigner"],
    isAnonymous: false,
  },
  {
    path: "/test/:id",
    name: "Test",
    exact: true,
    element: ResultViewer,
    roles: ["Admin", "Assigner"],
    isAnonymous: false,
  },
];

export default tests;
