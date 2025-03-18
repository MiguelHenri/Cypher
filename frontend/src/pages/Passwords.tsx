import { useEffect, useState } from "react";
import {
  Paper,
  Button,
  IconButton,
  List,
  ListItem,
  ListItemText,
  Typography,
  Stack,
  CircularProgress,
} from "@mui/material";
import DeleteIcon from "@mui/icons-material/Delete";
import EditIcon from "@mui/icons-material/Edit";
import AddIcon from "@mui/icons-material/Add";
import axios from "axios";
import { useAuth } from "../contexts/useAuth";

type PasswordItem = {
    id: number;
    serviceName: string;
    hashedPassword: string;
    createdAt: Date;
};

export default function Passwords() {
    const { token } = useAuth();
    const [loading, setLoading] = useState<boolean>(true);
    const [passwords, setPasswords] = useState<PasswordItem[]>([]);

    useEffect(() => {
        axios.get('/api/passwords', {
            headers: {
                Authorization: `Bearer ${token}`,
            }
        })
            .then((res) => {
                const processed: PasswordItem[] = res.data
                    .map(
                        (m: any, index: number) => ({
                            id: index,
                            serviceName: m.serviceName,
                            hashedPassword: m.hashedPassword,
                            createdAt: new Date(m.createdAt),
                        })
                    );
                setPasswords(processed);
            })
            .catch((err) => {
                console.error(err);
            })
            .finally(() => setLoading(false));
    }, []);

    const handleDelete = (id: number) => {
        console.log('delete', id);
    };

    const handleEdit = (id: number) => {
        console.log('edit', id);
    };

    const handleAdd = () => {
        console.log('add');
    };

    return (
        <Paper elevation={3} sx={{ padding: 4 }}>
            <Stack direction="row" justifyContent="center" alignItems="center" gap={3} mb={3}>
                <Typography variant="h5"> My Passwords </Typography>
                <Button
                    variant="contained"
                    startIcon={<AddIcon />}
                    onClick={handleAdd}
                >
                    New Password
                </Button>
            </Stack>
            <List>
                {loading ?
                <CircularProgress sx={{ alignSelf: "center"}}/>
                : !passwords.length ?
                <Typography align="center"> No passwords found. </Typography>
                :
                passwords.map((item) => (
                <ListItem
                    key={item.id}
                    secondaryAction={
                    <Stack direction="row" spacing={1}>
                        <IconButton edge="end" onClick={() => handleEdit(item.id)}>
                        <EditIcon />
                        </IconButton>
                        <IconButton edge="end" onClick={() => handleDelete(item.id)}>
                        <DeleteIcon />
                        </IconButton>
                    </Stack>
                    }
                >
                    <ListItemText primary={item.serviceName} />
                </ListItem>
                ))}
            </List>
        </Paper>
    );
}
