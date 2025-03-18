import { useState } from "react";
import { Button, Paper, TextField, Typography } from "@mui/material";
import axios from "axios";
import { useNavigate } from "react-router";

export default function Signup() {
    const navigate = useNavigate();
    const [email, setEmail] = useState<string>('');
    const [userName, setUserName] = useState<string>('');
    const [password, setPassword] = useState<string>('');
    const [passwordConf, setPasswordConf] = useState<string>('');

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        axios.post('/api/users/register', {
            email: email,
            name: userName,
            hashedPassword: password,
        }).then((res) => {
            if (res.status === 200) {
                navigate('/');
            }
            else {
                console.log(res);
            }
        }).catch((err) => {
            console.error(err);
        })
    };

    return (
        <Paper elevation={3} sx={{ padding: 4, width: 300 }}>
            <Typography variant="h5" align="center" gutterBottom>
                Sign Up
            </Typography>
            <form onSubmit={handleSubmit}>
                <TextField
                    label="Email"
                    type="email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    fullWidth
                    margin="normal"
                    required
                />
                <TextField
                    label="Name"
                    type="name"
                    value={userName}
                    onChange={(e) => setUserName(e.target.value)}
                    fullWidth
                    margin="normal"
                    required
                />
                <TextField
                    label="Password"
                    type="password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    fullWidth
                    margin="normal"
                    required
                />
                <TextField
                    label="Confirm Password"
                    type="password"
                    value={passwordConf}
                    onChange={(e) => setPasswordConf(e.target.value)}
                    fullWidth
                    margin="normal"
                    required
                />
                <Button
                    type="submit"
                    variant="contained"
                    color="primary"
                    fullWidth
                    sx={{ marginTop: 2 }}
                >
                    Sign Up
                </Button>
            </form>
            {/* Login Link */}
            <Typography variant="body2" align="center" sx={{ marginTop: 2 }}>
                Already have an account?{' '}
                <a href="/">
                    Login
                </a>.
            </Typography>
        </Paper>
    )
}