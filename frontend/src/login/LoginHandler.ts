interface LoginCredentials {
    username: string;
    password: string;
  }

  const LoginHandler = ({ username, password }: LoginCredentials) => {
    const storedUser = localStorage.getItem('user');
  
    if (!storedUser) {
      alert('No account found. Please sign up first.');
      return;
    }

    const user = JSON.parse(storedUser);

    if (user.username === username && user.password === password) {
        alert('Login successful!');
        localStorage.setItem('token', 'fake-jwt-token');
        window.location.href = '/dashboard';
      } else {
        alert('Invalid credentials. Please try again.');
      }
};

export default LoginHandler;