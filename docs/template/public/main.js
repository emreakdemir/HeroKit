export default {
  defaultTheme: 'auto',
  iconLinks: [
    { icon: 'github', href: 'https://github.com/emreakdemir/HeroKit', title: 'GitHub' },
    { icon: 'box-seam', href: 'https://www.nuget.org/packages/HeroKit', title: 'NuGet' },
  ],
  start: () => {
    // Copy buttons for the install commands on the landing page.
    document.querySelectorAll('[data-copy]').forEach(button => {
      button.addEventListener('click', async () => {
        await navigator.clipboard.writeText(button.dataset.copy);
        const label = button.textContent;
        button.textContent = 'Copied';
        setTimeout(() => (button.textContent = label), 1500);
      });
    });
  },
};
