document.addEventListener('DOMContentLoaded', function () {
    const whirlpoolScene = document.querySelector('.whirlpool-scene');

    if (!whirlpoolScene) {
        return;
    }

    whirlpoolScene.addEventListener('pointermove', function (event) {
        const rect = whirlpoolScene.getBoundingClientRect();
        const x = ((event.clientX - rect.left) / rect.width) * 100;
        const y = ((event.clientY - rect.top) / rect.height) * 100;

        const tiltX = (50 - y) * 0.08;
        const tiltY = (x - 50) * 0.08;

        whirlpoolScene.style.setProperty('--tilt-x', `${tiltX}deg`);
        whirlpoolScene.style.setProperty('--tilt-y', `${tiltY}deg`);
    });

    whirlpoolScene.addEventListener('pointerleave', function () {
        whirlpoolScene.style.setProperty('--tilt-x', '0deg');
        whirlpoolScene.style.setProperty('--tilt-y', '0deg');
    });
});
