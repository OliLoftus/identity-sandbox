import { useState } from 'react';
import { useAuth } from './auth/AuthProvider';
import './App.css';

interface Goal {
  id: string;
  title: string;
  description?: string;
}

type ApiResponse<T> = {
  status: string;
  message: string;
  data: T;
};

function App() {
  const { isAuthenticated, login, logout, user, isLoading } = useAuth();
  const [goals, setGoals] = useState<Goal[]>([]);

  const fetchGoals = async () => {
    if (!user?.access_token) {
      alert('You must be logged in to fetch goals!');
      return;
    }

    try {
      const response = await fetch('https://localhost:7108/api/Goal', {
        headers: {
          Authorization: `Bearer ${user.access_token}`,
        },
      });

      if (!response.ok) {
        throw new Error(`API call failed with status: ${response.status}`);
      }

      const json: ApiResponse<Goal[]> = await response.json();
      setGoals(json.data ?? []);
    } catch (error) {
      console.error(error);
      alert('Failed to fetch goals. Check the console for details.');
    }
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
            <button onClick={fetchGoals}>Fetch Goals</button>
            {goals.length > 0 && (
              <div>
                <h3>Your Goals:</h3>
                <ul>
                  {goals.map((goal) => (
                    <li key={goal.id}>
                      {goal.title}
                      {goal.description ? <span> — {goal.description}</span> : null}
                    </li>
                  ))}
                </ul>
              </div>
            )}
            <hr />
          </div>
        )}
      </header>
    </div>
  );
}

export default App;
