const { chromium } = require('playwright');

(async () => {
    try {
        console.log("Starting browser...");
        const browser = await chromium.launch();
        const page = await browser.newPage();
        console.log("Navigating to example.com...");
        await page.goto('https://example.com');
        const title = await page.title();
        console.log("Page title:", title);
        console.log("Example Domain");  // Assicurati che questa riga sia inclusa
        await browser.close();
    } catch (error) {
        console.error("An error occurred:", error);
    }
})();
