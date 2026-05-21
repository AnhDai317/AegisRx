// site.js
document.addEventListener('DOMContentLoaded', () => {
    const searchInput = document.getElementById('searchInput');
    const loadingSpinner = document.getElementById('loadingSpinner');
    const resultsList = document.getElementById('resultsList');
    const resultsTitle = document.getElementById('resultsTitle');

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
            // Call the local .NET API Proxy (running on 5263)
            const response = await fetch(`http://localhost:5263/api/medications/search?query=${encodeURIComponent(query)}`);
            
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }

            const data = await response.json();
            renderResults(data);
        } catch (error) {
            console.error('Error fetching data:', error);
            resultsList.innerHTML = '<div class="no-results">Error fetching data. Ensure API is running on port 5263.</div>';
        } finally {
            loadingSpinner.classList.add('hidden');
            resultsTitle.classList.remove('hidden');
        }
    }

    function renderResults(drugs) {
        resultsList.innerHTML = '';

        if (!drugs || drugs.length === 0) {
            resultsList.innerHTML = '<div class="no-results"><i class="ph ph-warning-circle" style="font-size: 2rem; margin-bottom: 10px; display:block;"></i>No medications found.</div>';
            return;
        }

        drugs.forEach(drug => {
            const card = document.createElement('div');
            card.className = 'drug-card';
            
            const brand = drug.brandName || 'Unknown Brand';
            const generic = drug.genericName || 'Unknown Generic';
            const ingredient = drug.activeIngredient || 'Not specified';

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

            resultsList.appendChild(card);
        });
    }
});
