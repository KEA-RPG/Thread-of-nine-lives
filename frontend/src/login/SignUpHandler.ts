interface SignUpCredentials {
    username: string;
    password: string;
}

const SignUpHandler = async ({ username, password }: SignUpCredentials) => {

    if (!username || !password) {
        alert('Username and password are required');
        return;
    }

    try {
        const response = await fetch('', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({})
        });

        if (response.ok) {
            const data = await response.json();
            console.log(`User ${data.username} signed up successfully`);
            window.location.href = '/';
            return data;
        } else {
            console.error('Error signing up');
        }
    } catch (error) {
        console.error('Request failed', error);
    }
};

export default SignUpHandler;