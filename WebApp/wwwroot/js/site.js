const setTheme = (theme) => {
    document.documentElement.setAttribute('data-bs-theme', theme);
    localStorage.setItem("theme", theme);
}

document.getElementById('toggleThemeBtn').addEventListener('click', () => {
    const currentTheme = document.documentElement.getAttribute('data-bs-theme');
    const newTheme = (currentTheme === 'dark') ? 'light' : 'dark';
    setTheme(newTheme);
});

window.addEventListener('DOMContentLoaded', () => {
    const storedTheme = localStorage.getItem("theme");
    if (storedTheme) {
        setTheme(storedTheme);
    } else {       
        setTheme('dark');
    }

    
    const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
    tooltipTriggerList.forEach((tooltipTriggerEl) => {
        new bootstrap.Tooltip(tooltipTriggerEl);
    });
});