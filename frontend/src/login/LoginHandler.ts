interface LoginCredentials {
    username: string;
    password: string;
}

const LoginHandler = async ({ username, password }: LoginCredentials) => {
    if (!username || !password) {
        alert('Please enter your username and password.');
        return;
    }

    try {
        const response = await fetch('', {
        method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({}),
        });

        if (!response.ok) {
            alert('Error fetching user data.');
            return;
        }

        const user = await response.json();

        if (user.username === username && user.password === password) {
            alert('Login successful!');
            window.location.href = '/main-navigation';
        } else {
            alert('Invalid credentials. Please try again.');
        }
    } catch (error) {
        console.error('Request failed', error);
    }
};

export default LoginHandler;