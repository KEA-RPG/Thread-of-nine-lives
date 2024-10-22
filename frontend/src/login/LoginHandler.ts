import { useEffect } from "react";
import { usePost } from "../hooks/useData";
import { UserContext } from "../components/UserContext";
import { jwtDecode } from "jwt-decode";

interface LoginCredentials {
    username: string;
    password: string;
}

const LoginHandler = async (credentials: LoginCredentials) => {
    const { username, password } = credentials;
    if (!username || !password) {
        alert('Please enter your username and password.');
        return;
    }
    useEffect(() => {
        const result = usePost<string, LoginCredentials>("auth/login", credentials);
        if (result.data) {
            const test = jwtDecode(result.data);
            console.log(result.data)
            console.log(test);
        } else {
            console.error('No data received');
        }
        console.log()
    })

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