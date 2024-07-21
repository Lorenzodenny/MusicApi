const { chromium } = require('playwright');

(async () => {
    const browser = await chromium.launch();
    const page = await browser.newPage();
    try {
        console.log("Waiting a bit before navigating...");
        await new Promise(resolve => setTimeout(resolve, 10000));
        console.log("Attempting to navigate to localhost:5050/Songs");
        await page.goto('http://localhost:5050/Songs', { waitUntil: 'networkidle' });
        console.log("Navigation successful");

        // GET
        const responseGet = await page.goto('http://localhost:5050/Songs');
        console.log("GET completed: " + (responseGet.ok() ? "Success" : "Failed"));

        // POST
        const responsePost = await page.request.post('http://localhost:5050/Songs', {
            data: JSON.stringify({ Name: 'New Song', Year: 2023, AlbumId: 1 }),
            headers: { 'Content-Type': 'application/json' }
        });
        console.log("POST completed: " + (responsePost.ok() ? "Success" : "Failed"));

        // PUT
        const responsePut = await page.request.put('http://localhost:5050/Songs/1', {
            data: JSON.stringify({ Name: 'Updated Song', Year: 2023, AlbumId: 1 }),
            headers: { 'Content-Type': 'application/json' }
        });
        console.log("PUT completed: " + (responsePut.ok() ? "Success" : "Failed"));

        // DELETE
        const responseDelete = await page.request.delete('http://localhost:5050/Songs/1');
        console.log("DELETE completed: " + (responseDelete.ok() ? "Success" : "Failed"));
    } catch (error) {
        console.error("An error occurred:", error);
    } finally {
        await browser.close();
    }
})();
