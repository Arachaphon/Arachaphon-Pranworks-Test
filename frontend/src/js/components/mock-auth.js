export default function mockAuth() {
  return {
    email: "",
    password: "",
    error: "",
    loading: false,

    login() {
      this.error = "";
      this.loading = true;

      setTimeout(() => {
        if (
          this.email.trim() === "Test@gmail.com" &&
          this.password === "Test_1234"
        ) {
          localStorage.setItem(
            "auth_user",
            JSON.stringify({
              username: "test_1",
              email: "Test@gmail.com",
            })
          );
          window.location.href = "index.html";
        } else {
          this.error = "อีเมลหรือรหัสผ่านไม่ถูกต้อง";
          this.loading = false;
        }
      }, 500);
    },
  };
}
