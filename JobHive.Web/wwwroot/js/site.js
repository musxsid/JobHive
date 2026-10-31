// JobHive Premium Interactive Scripts

document.addEventListener("DOMContentLoaded", function () {

    // ============================
    // 1. SPLASH SCREEN
    // ============================
    const splash = document.getElementById("splash-screen");
    if (splash && !sessionStorage.getItem("jhSplashShown")) {
        splash.style.display = "";
        setTimeout(() => {
            splash.classList.add("fade-out");
            setTimeout(() => { splash.style.display = "none"; }, 500);
        }, 3000);
        sessionStorage.setItem("jhSplashShown", "1");
    } else if (splash) {
        splash.style.display = "none";
    }

    // ============================
    // 2. SCROLL REVEAL ANIMATIONS
    // ============================
    const revealObserver = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                const el = entry.target;
                const delay = el.getAttribute("data-delay");
                if (delay) el.style.transitionDelay = delay + "ms";
                el.classList.add("active");
                revealObserver.unobserve(el);
            }
        });
    }, { threshold: 0.15 });

    document.querySelectorAll(".reveal, .reveal-left, .reveal-right").forEach(el => {
        revealObserver.observe(el);
    });

    // ============================
    // 3. NAVBAR SCROLL EFFECT & TOP PROGRESS BAR
    // ============================
    const navbar = document.querySelector(".navbar");
    const progressBar = document.getElementById("scroll-progress");
    if (navbar || progressBar) {
        const onScroll = () => {
            const scrollY = window.scrollY;
            if (navbar) navbar.classList.toggle("scrolled", scrollY > 40);
            if (progressBar) {
                const doc = document.documentElement;
                const total = doc.scrollHeight - doc.clientHeight;
                const pct = total > 0 ? (scrollY / total) * 100 : 0;
                progressBar.style.width = Math.min(pct, 100) + "%";
            }
        };
        window.addEventListener("scroll", onScroll, { passive: true });
        onScroll();
    }

    // ============================
    // 4. FLOATING CHATBOT WITH SUGGESTIONS & TYPING INDICATOR
    // ============================
    const chatFab = document.getElementById("chatbot-fab");
    const chatPanel = document.getElementById("chatbot-panel");

    const botResponses = {
        "hi": "Hello! Welcome to JobHive. How can I help you find your dream career today?",
        "hello": "Hello! Welcome to JobHive. What role or industry are you exploring?",
        "hey": "Hey there! Ready to take your career to the next level? Ask me anything!",
        "jobs": "You can explore all vacancies under 'Find Jobs'. Filter by location, salary brackets, and remote work to find your ideal match!",
        "remote": "Looking for flexible work? We have top remote opportunities in engineering, design, marketing, and customer success!",
        "apply": "Applying is super simple: open any vacancy, review the specifications, click 'Apply Now', attach your CV, and submit with a personal note!",
        "resume": "Manage your master resume under 'My Resume' in the navbar. Keeping it updated increases your profile visibility to recruiters!",
        "salary": "Compensation benchmarks range from 5 LPA to 35+ LPA across entry, mid-level, and senior positions."
    };
    const defaultReply = "Thanks for reaching out! Try exploring our job search filters or click 'My Resume' to keep your candidate profile updated for recruiters.";

    if (chatFab && chatPanel) {
        const chatBody = chatPanel.querySelector(".chatbot-body");
        const chatInput = chatPanel.querySelector(".chatbot-input");
        const chatSend = chatPanel.querySelector(".chatbot-send");

        const toggleChat = () => {
            const open = chatPanel.classList.toggle("open");
            chatFab.classList.toggle("active", open);
            if (open && chatInput) chatInput.focus();
        };

        chatFab.addEventListener("click", (e) => { e.stopPropagation(); toggleChat(); });

        document.addEventListener("click", (e) => {
            if (chatPanel.classList.contains("open") && !chatPanel.contains(e.target) && e.target !== chatFab) {
                chatPanel.classList.remove("open");
                chatFab.classList.remove("active");
            }
        });

        const appendBubble = (text, sender) => {
            if (!chatBody) return;
            const bubble = document.createElement("div");
            bubble.className = "chat-bubble " + sender;
            bubble.textContent = text;
            chatBody.appendChild(bubble);
            chatBody.scrollTop = chatBody.scrollHeight;
        };

        const showTypingIndicator = (callback) => {
            if (!chatBody) { callback(); return; }
            const indicator = document.createElement("div");
            indicator.className = "chat-bubble bot chat-typing-indicator";
            indicator.innerHTML = '<span></span><span></span><span></span>';
            chatBody.appendChild(indicator);
            chatBody.scrollTop = chatBody.scrollHeight;
            setTimeout(() => {
                indicator.remove();
                callback();
            }, 550);
        };

        const handleSend = (customMsg) => {
            const msg = customMsg || (chatInput ? chatInput.value.trim() : "");
            if (!msg) return;
            appendBubble(msg, "user");
            if (chatInput && !customMsg) chatInput.value = "";
            const key = msg.toLowerCase().replace(/[^a-z]/g, "");
            const reply = botResponses[key] || defaultReply;
            showTypingIndicator(() => appendBubble(reply, "bot"));
        };

        if (chatSend) chatSend.addEventListener("click", () => handleSend());
        if (chatInput) chatInput.addEventListener("keydown", (e) => { if (e.key === "Enter") handleSend(); });

        chatPanel.querySelectorAll(".chat-suggestion-chip").forEach(chip => {
            chip.addEventListener("click", () => {
                const key = chip.getAttribute("data-msg") || chip.textContent.trim();
                handleSend(chip.textContent.trim().replace(/^[^\w\s]+/, '').trim());
            });
        });
    }

    // ============================
    // 5. SMOOTH SCROLL FOR ANCHOR LINKS
    // ============================
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener("click", function (e) {
            const id = this.getAttribute("href");
            if (id === "#" || id.length < 2) return;
            const target = document.querySelector(id);
            if (target) {
                e.preventDefault();
                target.scrollIntoView({ behavior: "smooth", block: "start" });
            }
        });
    });

    // ============================
    // 6. COUNTER ANIMATION
    // ============================
    const counterObserver = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (!entry.isIntersecting) return;
            const el = entry.target;
            const target = parseInt(el.getAttribute("data-target"), 10);
            if (isNaN(target)) return;
            counterObserver.unobserve(el);
            const duration = 1600;
            const start = performance.now();
            const step = (now) => {
                const progress = Math.min((now - start) / duration, 1);
                const ease = 1 - Math.pow(1 - progress, 3);
                el.textContent = Math.floor(ease * target).toLocaleString();
                if (progress < 1) requestAnimationFrame(step);
                else el.textContent = target.toLocaleString();
            };
            requestAnimationFrame(step);
        });
    }, { threshold: 0.3 });

    document.querySelectorAll(".counter-value").forEach(el => counterObserver.observe(el));

    // ============================
    // 7. PARALLAX & 3D INTERACTIVE TILT
    // ============================
    const hero = document.querySelector(".hero-section");
    const heroInner = hero ? hero.querySelector(".hero-banner-wrapper") : null;
    if (hero && heroInner && window.innerWidth > 992) {
        hero.addEventListener("mousemove", (e) => {
            const rect = hero.getBoundingClientRect();
            const cx = (e.clientX - rect.left) / rect.width - 0.5;
            const cy = (e.clientY - rect.top) / rect.height - 0.5;
            heroInner.style.transform = `translate(${cx * 18}px, ${cy * 18}px)`;
        });
        hero.addEventListener("mouseleave", () => {
            heroInner.style.transform = "translate(0,0)";
        });
    }

    // Interactive 3D Card Hover Depth
    if (window.innerWidth > 992) {
        document.querySelectorAll(".category-card, .job-item-card, .process-step-card").forEach(card => {
            card.addEventListener("mousemove", (e) => {
                const rect = card.getBoundingClientRect();
                const x = e.clientX - rect.left;
                const y = e.clientY - rect.top;
                const xc = x / rect.width - 0.5;
                const yc = y / rect.height - 0.5;
                card.style.transform = `translateY(-6px) perspective(900px) rotateX(${yc * -6}deg) rotateY(${xc * 6}deg)`;
            });
            card.addEventListener("mouseleave", () => {
                card.style.transform = "";
            });
        });
    }

    // Dynamic Cursor Spotlight
    const spotlight = document.getElementById("cursor-spotlight");
    if (spotlight && window.innerWidth > 992) {
        let mouseX = window.innerWidth / 2;
        let mouseY = window.innerHeight / 2;
        let currentX = mouseX;
        let currentY = mouseY;

        document.addEventListener("pointermove", (e) => {
            mouseX = e.clientX;
            mouseY = e.clientY;
        }, { passive: true });

        const animateSpotlight = () => {
            currentX += (mouseX - currentX) * 0.12;
            currentY += (mouseY - currentY) * 0.12;
            spotlight.style.transform = `translate3d(${currentX - 300}px, ${currentY - 300}px, 0)`;
            requestAnimationFrame(animateSpotlight);
        };
        requestAnimationFrame(animateSpotlight);
    }

    // ============================
    // 8. BOOTSTRAP TOOLTIPS
    // ============================
    [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]')).forEach(el => {
        new bootstrap.Tooltip(el);
    });

    // ============================
    // 9. FILE INPUT PREVIEW
    // ============================
    document.querySelectorAll('input[type="file"]').forEach(input => {
        input.addEventListener("change", function () {
            if (!this.files || !this.files[0]) return;
            const name = this.files[0].name;
            const parent = this.closest(".border") || this.parentElement;
            const label = parent.querySelector(".file-selected-label");
            if (label) label.textContent = "Selected: " + name;
        });
    });

    // ============================
    // 10. ACTIVE NAV LINK HIGHLIGHT
    // ============================
    const currentPath = window.location.pathname.toLowerCase().replace(/\/+$/, "") || "/";
    document.querySelectorAll(".navbar-nav .nav-link").forEach(link => {
        const href = (link.getAttribute("href") || "").toLowerCase().replace(/\/+$/, "") || "/";
        if (href === currentPath || (href !== "/" && currentPath.startsWith(href))) {
            link.classList.add("active");
        }
    });

    // ============================
    // 11. BACK TO TOP BUTTON
    // ============================
    const topBtn = document.getElementById("back-to-top");
    if (topBtn) {
        window.addEventListener("scroll", () => {
            topBtn.classList.toggle("visible", window.scrollY > 300);
        }, { passive: true });
        topBtn.addEventListener("click", () => {
            window.scrollTo({ top: 0, behavior: "smooth" });
        });
    }
});
