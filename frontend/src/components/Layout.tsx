import { Outlet } from "react-router-dom";
import { Stack } from "@mui/material";

function Layout() {
  return (
    <Stack
      sx={{ 
        height: "95vh",
        alignItems: "center",
        justifyContent: "center"
      }}
    >
      <Outlet />
    </Stack>
  );
}

export default Layout;