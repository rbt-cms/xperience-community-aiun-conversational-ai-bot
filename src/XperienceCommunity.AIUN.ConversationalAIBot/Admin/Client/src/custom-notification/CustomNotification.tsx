import React, { useEffect } from 'react';

interface CustomNotificationProps {
    message: string;
    type?: 'success' | 'error';
    onClose: () => void;
}

export const CustomNotification: React.FC<CustomNotificationProps> = ({ message, type = 'success', onClose }) => {

    useEffect(() => {
        let timeout: NodeJS.Timeout;
        if (type === 'success') {
            timeout = setTimeout(() => {
                onClose();
            }, 3000); // Auto-dismiss after 3 seconds
        }
        return () => clearTimeout(timeout);
    }, [type, onClose]);



    if (type === 'error') {
        return (
            <div className="snackbar___OJH1o bottom-left___oWn6r vertical-l___qEjuC horizontal-l___VaUM6" data-testid="snackbar" data-ignored-by-clickoutside="true">
                <div className="snackbar-item___ygXXA item-bottom-left___uEOzU" data-testid="snackbar-item-error">
                    <div className="paper___e1CuL elevation-l___O_oOt" style={{ borderRadius: '8px' }}>
                        <div className="content-wrapper___GwQSD">
                            <div className="icon___VQ42x icon-error___rhksH">
                                <div data-testid="xp-times-circle">
                                    <svg width="1em" height="1em" viewBox="0 0 16 16" fill="none" xmlns="http://www.w3.org/2000/svg" role="img">
                                        <path d="M5.146 5.146a.5.5 0 0 1 .708 0L8 7.293l2.146-2.147a.5.5 0 0 1 .708.708L8.707 8l2.147 2.146a.5.5 0 0 1-.708.708L8 8.707l-2.146 2.147a.5.5 0 0 1-.708-.708L7.293 8 5.146 5.854a.5.5 0 0 1 0-.708Z" fill="currentColor"></path>
                                        <path fill-rule="evenodd" clip-rule="evenodd" d="M8 16A8 8 0 1 0 8 0a8 8 0 0 0 0 16Zm0-1A7 7 0 1 0 8 1a7 7 0 0 0 0 14Z" fill="currentColor"></path>
                                    </svg>
                                </div>
                            </div>
                            <div className="content___uPhUC" data-testid="snackbar-item-message">{message}</div>
                            <div className="divider___kxi8C">
                                <div className="divider___TSzOv vertical___sMz62"></div>
                            </div>
                            <div className="close___UpReY">
                                <div className="button-wrapper___wvgI6">
                                    <button onClick={onClose} className="button___Ky_Bj type-quinary___CtMYa size-s___oN26Z fill-container___I2NBK icon-only___fwQM4" type="button" aria-label="button" data-testid="button">
                                        <span className="icon___WmL0F">
                                            <div data-testid="xp-cancel">
                                                <svg width="1em" height="1em" viewBox="0 0 16 16" fill="none" xmlns="http://www.w3.org/2000/svg" role="img">
                                                    <path d="M1.146 14.147a.5.5 0 1 0 .707.707L8 8.707l6.147 6.148a.5.5 0 0 0 .707-.707L8.707 8l6.147-6.147a.5.5 0 0 0-.707-.707L8 7.293 1.853 1.146a.5.5 0 1 0-.707.708L7.293 8l-6.147 6.147Z" fill="currentColor"></path>
                                                </svg>
                                            </div>
                                        </span>
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
    else {
        return (
            <div className="snackbar___OJH1o bottom-left___oWn6r vertical-l___qEjuC horizontal-l___VaUM6" data-testid="snackbar" data-ignored-by-clickoutside="true">
                <div className="snackbar-item___ygXXA item-bottom-left___uEOzU" data-testid="snackbar-item-success">
                    <div className="paper___e1CuL elevation-l___O_oOt" style={{borderRadius: '8px'}}>
                        <div className="content-wrapper___GwQSD">
                            <div className="icon___VQ42x icon-success___vu1YD">
                                <div data-testid="xp-check-circle">
                                    <svg width="1em" height="1em" viewBox="0 0 16 16" fill="none" xmlns="http://www.w3.org/2000/svg" role="img">
                                        <path d="M11.785 6.237a.5.5 0 0 0-.707-.707l-4.45 4.45-1.706-1.705a.5.5 0 0 0-.707.707l2.059 2.06a.5.5 0 0 0 .707 0l4.804-4.805Z" fill="currentColor"></path>
                                        <path fill-rule="evenodd" clip-rule="evenodd" d="M8 16A8 8 0 1 0 8 0a8 8 0 0 0 0 16Zm7-8A7 7 0 1 1 1 8a7 7 0 0 1 14 0Z" fill="currentColor"></path>
                                    </svg>
                                </div>
                            </div>
                            <div className="content___uPhUC" data-testid="snackbar-item-message">{message}</div>
                            <div className="divider___kxi8C">
                                <div className="divider___TSzOv vertical___sMz62"></div>
                            </div>
                            <div className="close___UpReY">
                                <div className="button-wrapper___wvgI6">
                                    <button onClick={onClose} className="button___Ky_Bj type-quinary___CtMYa size-s___oN26Z fill-container___I2NBK icon-only___fwQM4" type="button" aria-label="button" data-testid="button">
                                        <span className="icon___WmL0F">
                                            <div data-testid="xp-cancel">
                                                <svg width="1em" height="1em" viewBox="0 0 16 16" fill="none" xmlns="http://www.w3.org/2000/svg" role="img">
                                                    <path d="M1.146 14.147a.5.5 0 1 0 .707.707L8 8.707l6.147 6.148a.5.5 0 0 0 .707-.707L8.707 8l6.147-6.147a.5.5 0 0 0-.707-.707L8 7.293 1.853 1.146a.5.5 0 1 0-.707.708L7.293 8l-6.147 6.147Z" fill="currentColor">
                                                    </path>
                                                </svg>
                                            </div>
                                        </span>
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        );
    }
    
};