import { useState } from "react";
import { Button, Paper, TextField, Typography } from "@mui/material";
import axios from "axios";
import { useNavigate } from "react-router";
import { useAuth } from "../contexts/useAuth";

export default function Home() {
    const navigate = useNavigate();
    const { saveLogin } = useAuth();
    const [email, setEmail] = useState<string>('');
    const [password, setPassword] = useState<string>('');

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        axios.post('/api/users/login', {
            email: email,
            hashedPassword: password,
        }).then((res) => {
            if (res.status === 200) {
                saveLogin(res.data.userName, res.data.token);
                console.log(res.data.userName, res.data.token);
                navigate('/passwords');
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
                Login
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
                    label="Password"
                    type="password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
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
                    Login
                </Button>
            </form>
        </Paper>
    )
}