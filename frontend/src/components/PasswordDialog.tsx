import { useState } from 'react';
import { Dialog, DialogTitle, DialogContent, DialogActions, Button, TextField, Typography } from '@mui/material';

export type DialogType = null | "create" | "edit" | "delete";

type PasswordDialogProps = {
    open: boolean;
    onClose: () => void;
    onSubmit: (() => void) | ((data: { serviceName: string; password: string }) => void);
    type: DialogType;
}

const PasswordDialog: React.FC<PasswordDialogProps> = ({ open, onClose, onSubmit, type }) => {
    const [serviceName, setServiceName] = useState<string>('');
    const [password, setPassword] = useState<string>('');

    const handleSubmit = () => {
        onSubmit({ serviceName, password });
        onClose();
        setServiceName('');
        setPassword('');
    };

    if (type === "delete") return (
        <Dialog open={open} onClose={onClose} fullWidth maxWidth="sm">
            <DialogTitle>Delete Confirmation</DialogTitle>
            <DialogContent>
                <Typography>Are you sure you want to delete the password?</Typography>
            </DialogContent>
            <DialogActions>
                <Button onClick={onClose}>Cancel</Button>
                <Button 
                    variant="contained" 
                    color="error" 
                    onClick={handleSubmit}
                >
                    Delete
                </Button>
            </DialogActions>
        </Dialog>
    )

    return (
        <Dialog open={open} onClose={onClose} fullWidth maxWidth="sm">
            <DialogTitle>{type == 'create' ? 'New ' : 'Edit '}Password</DialogTitle>
            <DialogContent>
                <TextField
                    margin="normal"
                    label="Service Name"
                    value={serviceName}
                    onChange={(e) => setServiceName(e.target.value)}
                    fullWidth
                    required
                />
                <TextField
                    margin="normal"
                    label="Password"
                    type="password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    fullWidth
                    required
                />
            </DialogContent>
            <DialogActions>
                <Button onClick={onClose}>Cancel</Button>
                <Button
                    variant="contained"
                    onClick={handleSubmit}
                    disabled={!serviceName || !password}
                >
                    Submit
                </Button>
            </DialogActions>
        </Dialog>
    );
};

export default PasswordDialog;
