import { useState } from 'react';
import { useAuth } from './auth/AuthProvider';
import './App.css';

interface Goal {
  id: number;
  text: string;
}

function App() {
  const { isAuthenticated, login, logout, user } = useAuth();
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

      const data = await response.json();
      setGoals(data);
    } catch (error) {
      console.error(error);
      alert('Failed to fetch goals. Check the console for details.');
    }
  };

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
                    <li key={goal.id}>{goal.text}</li>
                  ))}
                </ul>
              </div>
            )}
            <hr />
            <h3>Access Token:</h3>
            <pre
              style={{
                maxWidth: '80vw',
                overflowWrap: 'break-word',
                whiteSpace: 'pre-wrap',
                textAlign: 'left',
                fontSize: '0.7em',
              }}
            >
              {user?.access_token}
            </pre>
          </div>
        )}
      </header>
    </div>
  );
}

export default App;
