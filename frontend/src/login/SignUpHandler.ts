interface SignUpCredentials {
    username: string;
    password: string;
}

const SignUpHandler = ({ username, password }: SignUpCredentials) => {
    console.log(`Signing up with username: ${username} and password: ${password}`);

    if (!username || !password) {
        alert('Username and password are required');
        return;
    }

    alert(`User ${username} signed up successfully`);
    localStorage.setItem('user', JSON.stringify({ username, password }));
};

export default SignUpHandler;