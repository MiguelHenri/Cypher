import { useEffect, useState } from "react";
import {
  Box,
  Button,
  IconButton,
  List,
  ListItem,
  ListItemText,
  Typography,
  Stack,
} from "@mui/material";
import DeleteIcon from "@mui/icons-material/Delete";
import EditIcon from "@mui/icons-material/Edit";
import AddIcon from "@mui/icons-material/Add";

type PasswordItem = {
    id: number;
    serviceName: string;
};

export default function Passwords() {
    const [passwords, setPasswords] = useState<PasswordItem[]>([]);

    useEffect(() => {
        setPasswords([
            { id: 1, serviceName: "Google" },
            { id: 2, serviceName: "Facebook" },
            { id: 3, serviceName: "Twitter" },
        ]);
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
        <Box p={4}>
            <Stack direction="row" justifyContent="space-between" alignItems="center" mb={2}>
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
                {passwords.map((item) => (
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
        </Box>
    );
}
