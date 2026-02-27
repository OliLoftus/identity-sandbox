import { useState } from 'react';
import { useAuth } from './auth/AuthProvider';
import './App.css';

interface Goal {
  id: string;
  title: string;
  description?: string;
  category?: string;
  dueDate?: string;
  isCompleted?: boolean;
}

type ApiResponse<T> = {
  status: string;
  message: string;
  data: T;
  createdAt: string;
};

type PagedResponse<T> = ApiResponse<T> & {
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
};

function App() {
  const { isAuthenticated, login, logout, user, isLoading } = useAuth();
  const [goals, setGoals] = useState<Goal[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [isGoalsLoading, setIsGoalsLoading] = useState(false);

  const [category, setCategory] = useState('');
  const [titleContains, setTitleContains] = useState('');
  const [isCompleted, setIsCompleted] = useState('');
  const [dueFrom, setDueFrom] = useState('');
  const [dueTo, setDueTo] = useState('');
  const [sortBy, setSortBy] = useState('');
  const [sortDir, setSortDir] = useState('asc');
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [totalCount, setTotalCount] = useState(0);
  const [totalPages, setTotalPages] = useState(0);

  const buildQuery = () => {
    const params = new URLSearchParams();
    if (category.trim()) params.set('category', category.trim());
    if (titleContains.trim()) params.set('titleContains', titleContains.trim());
    if (isCompleted) params.set('isCompleted', isCompleted);
    if (dueFrom) params.set('dueFrom', dueFrom);
    if (dueTo) params.set('dueTo', dueTo);
    if (sortBy) params.set('sortBy', sortBy);
    if (sortDir) params.set('sortDir', sortDir);
    params.set('page', String(page));
    params.set('pageSize', String(pageSize));
    return params.toString();
  };

  const fetchGoals = async (nextPage?: number) => {
    if (!user?.access_token) {
      alert('You must be logged in to fetch goals!');
      return;
    }

    try {
      setIsGoalsLoading(true);
      setError(null);
      const targetPage = nextPage ?? page;
      if (targetPage !== page) {
        setPage(targetPage);
      }
      const query = buildQuery();
      const response = await fetch(`https://localhost:7108/api/Goal?${query}`, {
        headers: {
          Authorization: `Bearer ${user.access_token}`,
        },
      });

      if (!response.ok) {
        throw new Error(`API call failed with status: ${response.status}`);
      }

      const json: PagedResponse<Goal[]> = await response.json();
      setGoals(json.data ?? []);
      setTotalCount(json.totalCount ?? 0);
      setTotalPages(json.totalPages ?? 0);
      setPage(json.page ?? targetPage);
      setPageSize(json.pageSize ?? pageSize);
    } catch (error) {
      console.error(error);
      setError('Failed to fetch goals. Check the console for details.');
    } finally {
      setIsGoalsLoading(false);
    }
  };

  const applyFilters = async () => {
    setPage(1);
    await fetchGoals(1);
  };

  const clearFilters = async () => {
    setCategory('');
    setTitleContains('');
    setIsCompleted('');
    setDueFrom('');
    setDueTo('');
    setSortBy('');
    setSortDir('asc');
    setPage(1);
    setPageSize(10);
    await fetchGoals(1);
  };

  if (isLoading) {
    return (
      <div className="App">
        <header className="App-header">
          <p>Checking session...</p>
        </header>
      </div>
    );
  }

  return (
    <div className="App">
      <header className="App-header">
        <h1>Goal Tracker</h1>
        {!isAuthenticated ? (
          <button onClick={() => login()}>Log in</button>
        ) : (
          <div>
            <p>Hello, {user?.profile.name}</p>
            <button onClick={() => logout()}>Log out</button>
            <hr />
            <div className="controls">
              <div className="control-row">
                <label>
                  Category
                  <input
                    type="text"
                    value={category}
                    onChange={(event) => setCategory(event.target.value)}
                    placeholder="e.g. Fitness"
                  />
                </label>
                <label>
                  Title contains
                  <input
                    type="text"
                    value={titleContains}
                    onChange={(event) => setTitleContains(event.target.value)}
                    placeholder="e.g. run"
                  />
                </label>
                <label>
                  Status
                  <select value={isCompleted} onChange={(event) => setIsCompleted(event.target.value)}>
                    <option value="">All</option>
                    <option value="true">Completed</option>
                    <option value="false">Pending</option>
                  </select>
                </label>
              </div>
              <div className="control-row">
                <label>
                  Due from
                  <input type="date" value={dueFrom} onChange={(event) => setDueFrom(event.target.value)} />
                </label>
                <label>
                  Due to
                  <input type="date" value={dueTo} onChange={(event) => setDueTo(event.target.value)} />
                </label>
                <label>
                  Sort by
                  <select value={sortBy} onChange={(event) => setSortBy(event.target.value)}>
                    <option value="">None</option>
                    <option value="title">Title</option>
                    <option value="category">Category</option>
                    <option value="dueDate">Due date</option>
                    <option value="isCompleted">Completed</option>
                  </select>
                </label>
                <label>
                  Direction
                  <select value={sortDir} onChange={(event) => setSortDir(event.target.value)}>
                    <option value="asc">Asc</option>
                    <option value="desc">Desc</option>
                  </select>
                </label>
                <label>
                  Page size
                  <select value={pageSize} onChange={(event) => setPageSize(Number(event.target.value))}>
                    <option value={5}>5</option>
                    <option value={10}>10</option>
                    <option value={20}>20</option>
                    <option value={50}>50</option>
                  </select>
                </label>
              </div>
              <div className="control-row actions">
                <button onClick={applyFilters} disabled={isGoalsLoading}>
                  Apply filters
                </button>
                <button onClick={clearFilters} disabled={isGoalsLoading}>
                  Clear
                </button>
                <button onClick={() => fetchGoals()} disabled={isGoalsLoading}>
                  {isGoalsLoading ? 'Loading...' : 'Fetch goals'}
                </button>
              </div>
            </div>

            {error && <p className="error">{error}</p>}

            <div className="results">
              <div className="results-header">
                <h3>Your Goals</h3>
                <span>
                  {totalCount} total • page {page} of {totalPages || 1}
                </span>
              </div>
              {goals.length > 0 ? (
                <ul className="goals">
                  {goals.map((goal) => (
                    <li key={goal.id} className="goal-card">
                      <div className="goal-main">
                        <strong>{goal.title}</strong>
                        {goal.description ? <span>{goal.description}</span> : null}
                      </div>
                      <div className="goal-meta">
                        {goal.category ? <span>{goal.category}</span> : null}
                        {goal.dueDate ? <span>Due {new Date(goal.dueDate).toLocaleDateString()}</span> : null}
                        <span>{goal.isCompleted ? 'Completed' : 'Pending'}</span>
                      </div>
                    </li>
                  ))}
                </ul>
              ) : (
                <p>No goals yet. Try adjusting filters or create one.</p>
              )}
              <div className="pagination">
                <button
                  onClick={() => fetchGoals(Math.max(1, page - 1))}
                  disabled={page <= 1 || isGoalsLoading}
                >
                  Previous
                </button>
                <button
                  onClick={() => fetchGoals(page + 1)}
                  disabled={page >= (totalPages || 1) || isGoalsLoading}
                >
                  Next
                </button>
              </div>
            </div>
            <hr />
          </div>
        )}
      </header>
    </div>
  );
}

export default App;
