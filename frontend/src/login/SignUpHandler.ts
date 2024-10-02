interface SignUpCredentials {
    username: string;
    password: string;
}

const SignUpHandler = ({ username, password }: SignUpCredentials) => {

    if (!username || !password) {
        alert('Username and password are required');
        return;
    }

    alert(`User ${username} signed up successfully`);
    localStorage.setItem('user', JSON.stringify({ username, password }));
};

export default SignUpHandler;