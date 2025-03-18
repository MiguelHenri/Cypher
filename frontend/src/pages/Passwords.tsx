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
import PasswordDialog, { DialogType } from "../components/PasswordDialog";

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
    const [openDialog, setOpenDialog] = useState<DialogType>(null);

    useEffect(() => {
        axios.get('/api/passwords', {
            headers: {
                Authorization: `Bearer ${token}`,
            }
        })
            .then((res) => {
                const processed: PasswordItem[] = res.data
                    .map(
                        (m: any) => ({
                            id: m.id,
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

    const handleDelete = () => {
        console.log('delete');
    };

    const handleEdit = () => {
        console.log('edit');
    };

    const handleAdd = () => {
        console.log('add');
    };

    const getSubmitFunction = (type: DialogType) => {
        switch (type) {
            case "create":
                return handleAdd
            case "delete":
                return handleDelete
            case "edit":
                return handleEdit
            default:
                return () => {}
        }
    }

    return (
        <>
        <Paper elevation={3} sx={{ padding: 4 }}>
            <Stack direction="row" justifyContent="center" alignItems="center" gap={3} mb={3}>
                <Typography variant="h5"> My Passwords </Typography>
                <Button
                    variant="contained"
                    startIcon={<AddIcon />}
                    onClick={() => setOpenDialog("create")}
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
                        <IconButton edge="end" onClick={() => setOpenDialog("edit")}>
                        <EditIcon />
                        </IconButton>
                        <IconButton edge="end" onClick={() => setOpenDialog("delete")}>
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
        <PasswordDialog 
            open={openDialog !== null}
            onClose={() => setOpenDialog(null)}
            onSubmit={getSubmitFunction(openDialog)}
            type={openDialog}
        />
        </>
    );
}
