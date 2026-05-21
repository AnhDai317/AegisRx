// site.js
document.addEventListener('DOMContentLoaded', () => {
    const searchInput = document.getElementById('searchInput');
    const loadingSpinner = document.getElementById('loadingSpinner');
    const resultsList = document.getElementById('resultsList');
    const resultsTitle = document.getElementById('resultsTitle');
    
    // Modal Elements
    const modal = document.getElementById('addMedModal');
    const closeBtn = document.getElementById('closeModalBtn');
    const saveBtn = document.getElementById('saveMedBtn');
    const toast = document.getElementById('toast');

    let debounceTimer;

    searchInput.addEventListener('input', (e) => {
        const query = e.target.value.trim();
        clearTimeout(debounceTimer);

        if (query.length < 2) {
            resultsList.innerHTML = '';
            resultsTitle.classList.add('hidden');
            loadingSpinner.classList.add('hidden');
            return;
        }

        loadingSpinner.classList.remove('hidden');

        debounceTimer = setTimeout(() => {
            searchDrugs(query);
        }, 500);
    });

    async function searchDrugs(query) {
        try {
            const response = await fetch(`http://localhost:5263/api/medications/search?query=${encodeURIComponent(query)}`);
            if (!response.ok) throw new Error('Network error');
            const data = await response.json();
            renderResults(data);
        } catch (error) {
            resultsList.innerHTML = '<div class="no-results">Error fetching data. API might be down.</div>';
        } finally {
            loadingSpinner.classList.add('hidden');
            resultsTitle.classList.remove('hidden');
        }
    }

    function renderResults(drugs) {
        resultsList.innerHTML = '';

        if (!drugs || drugs.length === 0) {
            resultsList.innerHTML = '<div class="no-results">No medications found.</div>';
            return;
        }

        drugs.forEach(drug => {
            const card = document.createElement('div');
            card.className = 'drug-card';
            
            const brand = drug.brandName || 'Unknown Brand';
            const generic = drug.genericName || 'Unknown Generic';
            const ingredient = drug.activeIngredient || '';

            card.innerHTML = `
                <div class="drug-info">
                    <h3>${brand}</h3>
                    <p>Generic: <span>${generic}</span></p>
                    <p>Ingredient: ${ingredient}</p>
                </div>
                <button class="add-btn" title="Add to reminders">
                    <i class="ph ph-plus"></i>
                </button>
            `;

            // Attach event to the "+" button
            const btn = card.querySelector('.add-btn');
            btn.addEventListener('click', () => {
                openModal(brand, generic + (ingredient ? ' / ' + ingredient : ''));
            });

            resultsList.appendChild(card);
        });
    }

    // Modal Logic
    function openModal(brandName, description) {
        document.getElementById('modalDrugName').value = brandName;
        document.getElementById('modalDrugDesc').value = description;
        // Default expiry date to 1 year from now
        const nextYear = new Date();
        nextYear.setFullYear(nextYear.getFullYear() + 1);
        document.getElementById('modalExpiryDate').value = nextYear.toISOString().split('T')[0];
        document.getElementById('modalTime').value = "08:00";
        
        modal.classList.remove('hidden');
    }

    closeBtn.addEventListener('click', () => {
        modal.classList.add('hidden');
    });

    // Close if clicked outside
    window.addEventListener('click', (e) => {
        if (e.target == modal) {
            modal.classList.add('hidden');
        }
    });

    saveBtn.addEventListener('click', async () => {
        saveBtn.disabled = true;
        saveBtn.innerText = "Saving...";

        const payload = {
            name: document.getElementById('modalDrugName').value,
            description: document.getElementById('modalDrugDesc').value,
            expiryDate: document.getElementById('modalExpiryDate').value,
            reminders: [
                {
                    timeOfDay: document.getElementById('modalTime').value + ":00",
                    frequency: document.getElementById('modalFrequency').value,
                    timezoneId: Intl.DateTimeFormat().resolvedOptions().timeZone || "UTC"
                }
            ]
        };

        try {
            const response = await fetch('http://localhost:5263/api/medications', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload)
            });

            if (!response.ok) throw new Error('Failed to save');

            showToast("Medication added successfully!");
            modal.classList.add('hidden');
        } catch (error) {
            showToast("Failed to save medication.", true);
        } finally {
            saveBtn.disabled = false;
            saveBtn.innerText = "Save Medication";
        }
    });

    function showToast(message, isError = false) {
        toast.innerText = message;
        toast.className = 'toast';
        if (isError) toast.classList.add('error');
        
        toast.classList.remove('hidden');
        setTimeout(() => {
            toast.classList.add('hidden');
        }, 3000);
    }
});
